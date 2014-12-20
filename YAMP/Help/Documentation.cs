/*
    Copyright (c) 2012-2014, Florian Rappl.
    All rights reserved.

    Redistribution and use in source and binary forms, with or without
    modification, are permitted provided that the following conditions are met:
        * Redistributions of source code must retain the above copyright
          notice, this list of conditions and the following disclaimer.
        * Redistributions in binary form must reproduce the above copyright
          notice, this list of conditions and the following disclaimer in the
          documentation and/or other materials provided with the distribution.
        * Neither the name of the YAMP team nor the names of its contributors
          may be used to endorse or promote products derived from this
          software without specific prior written permission.

    THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
    ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
    WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
    DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
    DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
    (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
    LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
    ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
    (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
    SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Linq;
using System.Collections.Generic;
using YAMP;
using System.Reflection;
using System.Text;

namespace YAMP.Help
{
    /// <summary>
    /// Represents the documention in as an object.
    /// </summary>
	public class Documentation
	{
		#region Members

		List<HelpTopic> topics;
		ParseContext context;

		#endregion

		#region ctor

		Documentation(ParseContext _context)
		{
			context = _context;
			topics = new List<HelpTopic>();
		}

		#endregion

		#region Construction

        /// <summary>
        /// Creates a new documention instance from the given context.
        /// </summary>
        /// <param name="context">The context to use.</param>
        /// <returns>The documention.</returns>
		public static Documentation Create(ParseContext context)
		{
			var docu = new Documentation(context);
			docu.topics = Overview(context);
			return docu;
		}

        /// <summary>
        /// Gives an overview over the included functions and constants within the context.
        /// </summary>
        /// <param name="context">The context to investigate.</param>
        /// <returns>The list with help topics.</returns>
		public static List<HelpTopic> Overview(ParseContext context)
		{
			var topics = new List<HelpTopic>();
			var functions = context.AllFunctions;
			var constants = context.AllConstants;

			foreach (var function in functions)
			{
				AddTypeToTopics(function.Key, "Function", function.Value, topics);
			}

			foreach (var constant in constants)
			{
				AddTypeToTopics(constant.Key, "Constant", constant.Value, topics);
			}

			return topics;
		}

		#endregion

		#region Members

        /// <summary>
        /// Gets the access to an enumerable of all the HelpSections within the documention.
        /// </summary>
		public IEnumerable<HelpSection> Sections
		{
			get
			{
				return topics.SelectMany(m => m).Select(
					m => Get(m.Name)
				).OrderBy(m => m.Name).AsEnumerable();
			}
		}

        /// <summary>
        /// Gets the access to an enumerable of all HelpTopics within the documention.
        /// </summary>
		public IEnumerable<HelpTopic> Topics
		{
			get
			{
				return topics.OrderBy(m => m.Kind).AsEnumerable();
			}
		}

        /// <summary>
        /// Looks if a certain topic is contained within the documention.
        /// </summary>
        /// <param name="topic">The topic to look for.</param>
        /// <returns>The result of the search.</returns>
		public bool ContainsTopic(string topic)
		{
			foreach (var tp in topics)
				if (tp.Kind.Equals(topic, StringComparison.CurrentCultureIgnoreCase))
					return true;

			return false;
		}

        /// <summary>
        /// Looks if a certain entry is contained within the documention.
        /// </summary>
        /// <param name="entry">The entry's name to look for.</param>
        /// <returns>The result of the search.</returns>
		public bool ContainsEntry(string entry)
		{
			foreach (var tp in topics)
				foreach (var ti in tp)
					if (ti.Name.Equals(entry, StringComparison.CurrentCultureIgnoreCase))
						return true;

			return false;
		}

        /// <summary>
        /// Finds the closest entry to the given entry.
        /// </summary>
        /// <param name="entry">The (probably) mispelled entry's name.</param>
        /// <returns>The name of a correct entry that seems to be fairly close.</returns>
		public string ClosestEntry(string entry)
		{
			var term = entry.ToLower();
			var list = new List<string>();

			foreach (var tp in topics)
				foreach (var ti in tp)
					list.Add(ti.Name);

			if (list.Contains(term))
				return list.FirstOrDefault(t => t.ToLower() == term);
			else
			{
				var min = int.MaxValue;
				var index = 0;

				for (int i = 0; i < list.Count; i++)
				{
					var sum = Distance(term, list[i], 10);

					if (sum < min)
					{
						min = sum;
						index = i;
					}
				}

				return list[index];
			}
		}

        /// <summary>
        /// Gets the HelpSection that belongs to the name of the given entry.
        /// </summary>
        /// <param name="entry">The name of the entry to retrieve.</param>
        /// <returns>The HelpSection instance.</returns>
		public HelpSection Get(string entry)
		{
			var topic = topics.SelectMany(m => m.Where(n => n.Name.Equals(entry, StringComparison.CurrentCultureIgnoreCase)).Select(n => n)).FirstOrDefault();
			var to = topic.Instance.GetType();

			if (topic.Instance is IFunction)
			{
				var help = new HelpFunctionSection();
				help.Name = topic.Name;
				help.Description = GetDescription(to);
				help.Topic = topic.Topic.Kind;
                help.Link = GetLink(to);
				var functions = to.GetMethods();

				foreach (var function in functions)
				{
					if (function.Name.Equals("Function"))
					{
						var isextern = true;
						var parameters = function.GetParameters();

						foreach (var parameter in parameters)
						{
							if (!parameter.ParameterType.IsSubclassOf(typeof(Value)))
							{
								isextern = false;
								break;
							}
						}

						if (isextern)
							help.Usages.Add(GetUsage(help.Name, function));
					}
				}

				return help;
			}
			else
			{
				var help = new HelpSection();
				help.Name = topic.Name;
				help.Description = GetDescription(to);
                help.Topic = topic.Topic.Kind;
                help.Link = GetLink(to);
				return help;
			}
		}

		#endregion

		#region Helpers

		static int Distance(string s1, string s2, int maxOffset)
		{
			if (string.IsNullOrEmpty(s1))
				return string.IsNullOrEmpty(s2) ? 0 : s2.Length;

			if (string.IsNullOrEmpty(s2))
				return s1.Length;

			int c = 0;
			int offset1 = 0;
			int offset2 = 0;
			int lcs = 0;

			while ((c + offset1 < s1.Length) && (c + offset2 < s2.Length))
			{
				if (s1[c + offset1] == s2[c + offset2])
					lcs++;
				else
				{
					offset1 = 0;
					offset2 = 0;

					for (int i = 0; i < maxOffset; i++)
					{
						if ((c + i < s1.Length) && (s1[c + i] == s2[c]))
						{
							offset1 = i;
							break;
						}

						if ((c + i < s2.Length) && (s1[c] == s2[c + i]))
						{
							offset2 = i;
							break;
						}
					}
				}

				c++;
			}

			return (s1.Length + s2.Length) / 2 - lcs;
		}

		static void AddTypeToTopics(string name, string standardKind, object type, List<HelpTopic> topics)
		{
			var entry = new HelpEntry
			{
				Name = name,
				Instance = type
			};
			var kind = standardKind;
			var decl = type.GetType().GetCustomAttributes(typeof(KindAttribute), false);

			if (decl.Length > 0)
				kind = (decl[0] as KindAttribute).Kind;

			foreach (var topic in topics)
			{
				if (topic.Kind.Equals(kind))
				{
					entry.Topic = topic;
					topic.Add(entry);
					return;
				}
			}

			var ht = new HelpTopic(kind);
			entry.Topic = ht;
			ht.Add(entry);
			topics.Add(ht);
		}

        #endregion

        #region Get Information

        HelpFunctionUsage GetUsage(string name, MethodInfo function)
		{
			var objects = function.GetCustomAttributes(typeof(ExampleAttribute), false);
			var rets = function.GetCustomAttributes(typeof(ReturnsAttribute), false);
			var help = new HelpFunctionUsage();
			var args = function.GetParameters();

			if (rets.Length == 0)
				help.Returns.Add(ModifyValueType(function.ReturnType));
			else
			{
				rets = rets.OrderBy(m => ((ReturnsAttribute)m).Order).ToArray();

				foreach (ReturnsAttribute attribute in rets)
					help.Returns.Add(ModifyValueType(attribute.ReturnType) + " : " + attribute.Explanation);
			}

            help.Description = GetDescription(function);
            var sb = new StringBuilder();

            foreach (var arg in args)
            {
                help.ArgumentNames.Add(arg.Name);
                help.Arguments.Add(ModifyValueType(arg.ParameterType));
            }

            sb.Append(name).Append("(");
            sb.Append(string.Join(",", help.ArgumentNames.ToArray()));
            sb.AppendLine(")");
            help.Usage = sb.ToString();

			foreach (ExampleAttribute attribute in objects)
				help.Examples.Add(GetExample(attribute));

			return help;
		}

		HelpExample GetExample(ExampleAttribute attribute)
		{
			var help = new HelpExample();
			help.Example = attribute.Example;
			help.Description = attribute.Description;
            help.IsFile = attribute.IsFile;
			return help;
		}

		string GetDescription(MemberInfo element)
		{
			var objects = element.GetCustomAttributes(typeof(DescriptionAttribute), false);

			if (objects.Length == 0)
				return "No description available.";

			var sb = new StringBuilder();

			foreach (DescriptionAttribute attribute in objects)
				sb.AppendLine(attribute.Description);

			return sb.ToString();
		}

        string GetLink(MemberInfo element)
        {
            var objects = element.GetCustomAttributes(typeof(LinkAttribute), false);

            if (objects.Length == 0)
                return string.Empty;

            return ((LinkAttribute)objects[0]).Url;
        }

		string ModifyValueType(Type type)
		{
			return type.Name.RemoveValueConvention();
		}

		#endregion
	}
}
