using System;
using System.ComponentModel;

namespace YAMP
{
    class AsyncTask : BackgroundWorker
    {
        public Action<Value, Exception> Continuation { get; set; }
    }
}
