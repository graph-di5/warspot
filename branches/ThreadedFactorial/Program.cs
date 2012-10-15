using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

//Конечный вариант написан по руководству http://www.rsdn.ru/article/dotnet/CSThreading1.xml
namespace ThreadedFactorial
{
	class Task//struct не ссылочный тип
	{
		public System.Numerics.BigInteger taskNumber;
		public System.Numerics.BigInteger from;
		public System.Numerics.BigInteger to;
		public System.Numerics.BigInteger result;

		public Task()
		{
			taskNumber = 0;
			from = 0;
			to = 0;
			result = 0;
		}

		public Task(System.Numerics.BigInteger _taskNumber, System.Numerics.BigInteger _from, System.Numerics.BigInteger _to)
		{
			taskNumber = _taskNumber;
			from = _from;
			to = _to;
			result = 0;
		}
	}

	class Queue : IDisposable
	{
		public Task exit = new Task();//для завершения потоков.

		EventWaitHandle waitHandle = new AutoResetEvent(false);
		Thread worker;
		object locker = new object();
		Queue<Task> waitingTasks = new Queue<Task>();

		public Queue()
		{
			worker = new Thread(Calculator);
			worker.Start();
		}

		public void EnqueueTask(Task task)
		{
			lock (locker)
				waitingTasks.Enqueue(task);

			waitHandle.Set();
		}

		public void Dispose()
		{
			EnqueueTask(exit);
			worker.Join();
			waitHandle.Close();
		}

		void Calculator()
		{
			while (true)
			{
				System.Numerics.BigInteger tempNumber;
				System.Numerics.BigInteger tempResult = 1;
				Task task = exit;

				lock (locker)
				{
					if (waitingTasks.Count > 0)
					{
						task = waitingTasks.Dequeue();
						if (task.Equals(exit))
							return;
					}
				}

				if (!task.Equals(exit))
				{
					for (tempNumber = task.from; tempNumber <= task.to; tempNumber++)
						tempResult *= tempNumber;
					
					task.result = tempResult;
				}
				else
					waitHandle.WaitOne();			
			}
		}
	}


	class Program
	{
		
		
		static void Main(string[] args)
		{			
			const int THREADS_PER_PROCESSOR = 4;
			const int TASK_PER_THREAD = 4;
			System.Numerics.BigInteger threadCount = THREADS_PER_PROCESSOR * System.Environment.ProcessorCount;
			System.Numerics.BigInteger maxNumber = 0;
			List<Task> mainTaskList = new List<Task>();
			
			System.Console.WriteLine("Enter the maximum number: ");
			while (maxNumber < threadCount * 4)
			{
				while (!System.Numerics.BigInteger.TryParse(System.Console.ReadLine(), out maxNumber))
					System.Console.WriteLine("Can't parse the number... Reenter: ");

				if (maxNumber < threadCount * 4)
					System.Console.WriteLine("Max number is too small. Reenter: ");
			}
			Console.WriteLine("Computing...");
			
			int start = Environment.TickCount;//засекаем время
					
			System.Numerics.BigInteger part = maxNumber/(threadCount*TASK_PER_THREAD);
			System.Numerics.BigInteger taskNumber = 0;
			System.Numerics.BigInteger from = 0;
			System.Numerics.BigInteger to = 0;
			using (Queue q = new Queue())
			{
				for (; from < maxNumber - 1; from = to)
				{
					to = from + part;
					if (to > maxNumber)
						to = maxNumber;
					Task newTask = new Task(taskNumber, from+1, to);
					mainTaskList.Add(newTask);
					q.EnqueueTask(newTask);
					taskNumber++;
				}//Создали список задач и очередь задач
				
				if (to!=maxNumber)
				{
					Task newTask = new Task(taskNumber, maxNumber, maxNumber);
					mainTaskList.Add(newTask);
					newTask.result = maxNumber;
					taskNumber++;
				}//Если последнее число не уместилось
			}			

			System.Numerics.BigInteger completeResult = 1;
			foreach (Task tempTask in mainTaskList)
			{
				completeResult *= tempTask.result;
			}

			int duration = Environment.TickCount - start;//получаем времы выполнения
			//foreach (Task tempTask in mainTaskList)
			//{
			//    Console.WriteLine("Task {0} from {1} to {2}. Result {3}", tempTask.taskNumber.ToString(), tempTask.from.ToString(), tempTask.to.ToString(), tempTask.result.ToString());
			//}
			Console.WriteLine("Complete result: {0}", completeResult);
			Console.WriteLine("It is the result /\\");
			Console.WriteLine("That took {0} ms.", duration);
			Console.ReadKey();
		}
	}
}