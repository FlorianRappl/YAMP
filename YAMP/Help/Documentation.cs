using System;
using System.Linq;
using System.Collections.Generic;
using YAMP;
using System.Reflection;
using System.Text;

namespace YAMP.Help
{
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

		public static Documentation Create(ParseContext context)
		{
			var docu = new Documentation(context);
			docu.topics = Overview(context);
			return docu;
		}

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

		public IEnumerable<HelpSection> Sections
		{
			get
			{
				return topics.SelectMany(m => m).Select(
					m => Get(m.Name)
				).OrderBy(m => m.Name).AsEnumerable();
			}
		}

		public IEnumerable<HelpTopic> Topics
		{
			get
			{
				return topics.OrderBy(m => m.Kind).AsEnumerable();
			}
		}

		public bool ContainsTopic(string topic)
		{
			foreach (var tp in topics)
				if (tp.Kind.Equals(topic, StringComparison.CurrentCultureIgnoreCase))
					return true;

			return false;
		}

		public bool ContainsEntry(string entry)
		{
			foreach (var tp in topics)
				foreach (var ti in tp)
					if (ti.Name.Equals(entry, StringComparison.CurrentCultureIgnoreCase))
						return true;

			return false;
		}

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
				return help;
			}
		}

		#endregion

		#region Helpers

		int Distance(string s1, string s2, int maxOffset)
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

		HelpFunctionUsage GetUsage(string name, MethodInfo function)
		{
			var objects = function.GetCustomAttributes(typeof(ExampleAttribute), false);
			var rets = function.GetCustomAttributes(typeof(ReturnsAttribute), false);
			var help = new HelpFunctionUsage();
			var args = function.GetParameters();

			var sb = new StringBuilder();
			sb.Append(name).Append("(");
			var s = new string[args.Length];

			for (var i = 0; i < args.Length; )
				s[i] = "x" + (++i);

			sb.Append(string.Join(",", s));
			sb.AppendLine(")");

			help.Usage = sb.ToString();

			if (rets.Length == 0)
				help.Returns.Add(ModifyValueType(function.ReturnType));
			else
			{
				rets = rets.OrderBy(m => ((ReturnsAttribute)m).Order).ToArray();

				foreach (ReturnsAttribute attribute in rets)
					help.Returns.Add(ModifyValueType(attribute.ReturnType) + " : " + attribute.Explanation);
			}

			help.Description = GetDescription(function);

			foreach (var arg in args)
				help.Arguments.Add(ModifyValueType(arg.ParameterType));

			foreach (ExampleAttribute attribute in objects)
				help.Examples.Add(GetExample(attribute));

			return help;
		}

		HelpExample GetExample(ExampleAttribute attribute)
		{
			var help = new HelpExample();
			help.Example = attribute.Example;
			help.Description = attribute.Description;
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

		string ModifyValueType(Type type)
		{
			return type.Name.RemoveValueConvention();
		}

		#endregion
	}
}
