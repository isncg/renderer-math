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
        //public Vector3[,] result;
        public int xMin, xMax, yMin, yMax;
        Thread renderThread = null;
        public RenderTaskManager mgr= null;
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
            for (int i = yMin; i < yMax; i++)
            {
                for (int j = xMin; j < xMax; j++)
                {
                    Vector3 result = camera.Trace(camera.pixelRays[i, j], triangles, depth);
                    mgr.OnPixelFinish(ID, i, j, result);
                    //if (pixelFinishCallback != null)
                    //{
                    //    pixelFinishCallback(ID, i, j, result);
                    //}
                }
            }
        }
    }

    class RenderTaskManager
    {
        public int[] pxTotal;
        public int[] pxFinished;
        public Vector3[,] result;
        List<RenderTask> taskList = null;
        public DateTime startTime;
        public DateTime endTime;
        public void OnPixelFinish(int id, int i, int j, Vector3 result)
        {
            Thread.BeginCriticalRegion();
            this.result[i, j] = result;
            StringBuilder b = new StringBuilder();
            pxFinished[id]++;
            for (int n = 0; n < pxTotal.Length; n++)
            {
                b.Append(string.Format("T-{0}: {1:00.000}% ", n, (float)pxFinished[n] * 100 / (float)pxTotal[n]));
            }
            //Console.Write(b);
            //Console.WriteLine("    {0:000} {1:000} {2}", i, j, result);
            Console.Write("\r{0}  ", b);

            Thread.EndCriticalRegion();
        }
        public void CreateTask(int nThreads, Camera camera, List<Triangle> triangles, int depth)
        {
            pxTotal = new int[nThreads];
            pxFinished = new int[nThreads];
            result = new Vector3[camera.height, camera.width];
            int i = 0;
            for (i = 0; i < nThreads; i++)
            {
                pxTotal[i] = 0;
                pxFinished[i] = 0;
            }
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
                t.mgr = this;
                taskList.Add(t);


                pxTotal[i] = (t.xMax - t.xMin) * (t.yMax - t.yMin);
            }
        }

        public Vector3[,] Run()
        {
            int w = Console.BufferWidth;
            startTime = DateTime.Now;
            foreach (var t in taskList)
            {
                t.Start();
            }
            foreach (var t in taskList)
            {
                t.Wait();
            }
            endTime = DateTime.Now;
            Console.Write("\r");
            for (int i = 0; i < w; i++)
            {
                Console.Write(" ");
            }
            Console.Write("\r");
            return result;
        }
    }
}
