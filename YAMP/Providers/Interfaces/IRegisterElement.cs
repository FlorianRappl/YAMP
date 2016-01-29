using System;

namespace YAMP
{
    /// <summary>
    /// This interface is required to indicate that the object should be (if possible)
    /// taken, instantiated and registred (with the method) at loading.
    /// </summary>
	interface IRegisterElement
	{
        /// <summary>
        /// Register an element somewhere automatically.
        /// </summary>
		void RegisterElement();
	}
}

