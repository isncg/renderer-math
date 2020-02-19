using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace RendererMath
{
    class RenderTask
    {
        public int ID;
        public Camera camera;
        public List<Triangle> triangles;
        public int depth;
        public Vector3[,] result;
        public int xMin, xMax, yMin, yMax;
        Thread renderThread = null;
        public float progress = 0;
        public Action NotifyProgress = null;
        //public Random random;
        //public RenderTaskManager mgr= null;
        public void Start()
        {
            if (renderThread == null)
            {
                renderThread = new Thread(Proc);
                renderThread.Start();
            }
        }

        public void Wait()
        {
            renderThread.Join();
            renderThread = null;
        }

        public void Proc()
        {
            result = new Vector3[camera.height, camera.width];
            //Random random = new Random();
            int i, j;
            var zero = new ZeroVector3();
            for (i = 0; i < camera.height; i++)
            {
                for (j = 0; j < camera.width; j++)
                {
                    result[i, j] = zero;
                }
            }


            for (i = yMin; i < yMax; i++)
            {
                progress = (float)(i - yMin) / (yMax - yMin);
                NotifyProgress();
                for (j = xMin; j < xMax; j++)
                {
                    result[i, j] = camera.Trace(camera.pixelRays[i, j], triangles, depth);
                    //mgr.OnPixelFinish(ID, i, j, result);
                    //if (pixelFinishCallback != null)
                    //{
                    //    pixelFinishCallback(ID, i, j, result);
                    //}
                }               
            }
            progress = (float)(i - yMin) / (yMax - yMin);
            NotifyProgress();
        }
    }

    class RenderTaskManager
    {
        //public int[] pxTotal;
        //public int[] pxFinished;
        //public Vector3[,] result;
        public List<RenderTask> taskList = null;
        public DateTime startTime;
        public DateTime endTime;
        Camera camera = null;
        //public void OnPixelFinish(int id, int i, int j, Vector3 result)
        //{
        //    Thread.BeginCriticalRegion();
        //    this.result[i, j] = result;
        //    StringBuilder b = new StringBuilder();
        //    pxFinished[id]++;
        //    for (int n = 0; n < pxTotal.Length; n++)
        //    {
        //        b.Append(string.Format("T-{0}: {1:00.000}% ", n, (float)pxFinished[n] * 100 / (float)pxTotal[n]));
        //    }
        //    //Console.Write(b);
        //    //Console.WriteLine("    {0:000} {1:000} {2}", i, j, result);
        //    Console.Write("\r{0}  ", b);

        //    Thread.EndCriticalRegion();
        //}

        public void NotifyProgress()
        {
            lock (this)
            {
                StringBuilder b = new StringBuilder();
                for(int i=0; i<taskList.Count; i++)
                {
                    b.Append(string.Format("T-{0}: {1:00.000}% ", i, taskList[i].progress*100));
                }
                Console.Write("\r{0}  ", b);
            }
        }

        public void CreateTask(int nThreads, Camera camera, List<Triangle> triangles, int depth)
        {
            //pxTotal = new int[nThreads];
            //pxFinished = new int[nThreads];
            this.camera = camera;
            //result = new Vector3[camera.height, camera.width];
            int i = 0;
            //for (i = 0; i < nThreads; i++)
            //{
            //    pxTotal[i] = 0;
            //    pxFinished[i] = 0;
            //}
            taskList = new List<RenderTask>();
            for (i = 0; i < nThreads; i++)
            {
                RenderTask t = new RenderTask();
                t.ID = i;
                t.camera = camera;
                t.triangles = triangles;
                //t.result = result;
                t.yMin = 0;
                t.yMax = camera.height;
                t.xMin = camera.width * i / nThreads;
                t.xMax = camera.width * (i + 1) / nThreads;
                t.depth = depth;
                //t.mgr = this;
                t.NotifyProgress = this.NotifyProgress;
                taskList.Add(t);


                //pxTotal[i] = (t.xMax - t.xMin) * (t.yMax - t.yMin);
            }
        }

        public Vector3[,] Run(int[] threadIDs = null)
        {
            int w = Console.BufferWidth;
            startTime = DateTime.Now;
            if (threadIDs == null)
            {
                foreach (var t in taskList)
                {
                    t.Start();
                }
                foreach (var t in taskList)
                {
                    t.Wait();
                }
            }
            else
            {
                foreach(var id in threadIDs)
                {
                    taskList[id].Start();
                }
                foreach (var id in threadIDs)
                {
                    taskList[id].Wait();
                }
            }
            endTime = DateTime.Now;
            Console.Write("\r");
            for (int i = 0; i < w; i++)
            {
                Console.Write(" ");
            }
            Console.Write("\r");
            var result = new Vector3[camera.height, camera.width];

            foreach(var t in taskList)
            {
                if (t.result != null)
                {
                    for (int i = t.yMin; i < t.yMax; i++)
                    {
                        for (int j = t.xMin; j < t.xMax; j++)
                        {
                            result[i, j] = t.result[i, j];
                        }
                    }
                }
            }

            return result;
        }
    }
}
