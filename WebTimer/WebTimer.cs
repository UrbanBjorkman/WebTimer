using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace WebTimer
{
	public class BackgroundHelper
	{
		private class JobHost : IRegisteredObject
		{
			public JobHost()
			{
				HostingEnvironment.RegisterObject(this);
			}

			public void Stop(bool immediate)
			{
				HostingEnvironment.UnregisterObject(this);
			}

			public void DoWork(Action work)
			{
				work();
			}
		}

		public static void RunBackgroundJob(Action func, int intDelayMs = 0)
		{
			var thread = new Thread(() =>
			{
				try
				{
					var host = new JobHost();
					if (intDelayMs > 0)
					{
						host.DoWork(() =>
						{
							//Initial delay
							Thread.Sleep(intDelayMs);
							func();
						});
					}
					else
					{
						host.DoWork(func);
					}
				}
				catch (Exception ex)
				{
					//TODO: Errorhandling
					throw ex;
				}

			});
			thread.Start();
		}

	}
}