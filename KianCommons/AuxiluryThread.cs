using System;
using System.Collections.Generic;
using System.Threading;

namespace KianCommons;

internal static class AuxiluryThread
{
	private static Thread thread_;

	private static Queue<Action> tasks_;

	static AuxiluryThread()
	{
		tasks_ = new Queue<Action>(1023);
		Start();
	}

	public static void EnqueueAction(Action act)
	{
		lock (tasks_)
		{
			tasks_.Enqueue(act);
		}
	}

	private static Action DequeueAction()
	{
		lock (tasks_)
		{
			if (tasks_.Count == 0)
			{
				return null;
			}
			return tasks_.Dequeue();
		}
	}

	public static void Start()
	{
		thread_ = new Thread(ThreadTask);
		thread_.IsBackground = true;
		thread_.Name = typeof(AuxiluryThread).Assembly.Name();
		thread_.Priority = ThreadPriority.Lowest;
		thread_.Start();
	}

	public static void End()
	{
		thread_.Abort();
	}

	private static void ThreadTask()
	{
		while (true)
		{
			try
			{
				Action action = DequeueAction();
				if (action != null)
				{
					action();
				}
				else
				{
					Thread.Sleep(100);
				}
			}
			catch (Exception ex)
			{
				ex.Exception();
			}
		}
	}
}
