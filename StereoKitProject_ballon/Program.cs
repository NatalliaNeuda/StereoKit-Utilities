using System;
using StereoKit;
using System.Collections.Generic;
using System.Linq;

namespace StereoKitProject_ballon
{
    class ObjectSet
    {
        Model sphereModel;
        float size = 0.3f;
        string time1 = "";
        bool game_over = false;
        bool start = false;
          List <Solid> objects = new List<Solid>();

        public ObjectSet()
        {
            sphereModel = Model.FromMesh(
                   Mesh.GenerateSphere(size, 20),
                   Default.Material);
            sphereModel.AddSubset(Mesh.GenerateCylinder(0.01f, 1, new Vec3(0, -1, 0)), Default.Material, Matrix.T(0, -0.6f, 0));
        }
        public void Update()
        {
            DisplayButtonPanel();
            BlownObject();
        }
        
        private void BlownObject()
        {
            
            Hand rightHand = Input.Hand(Handed.Right);
            Vec3 position;

            var style = Text.MakeStyle(
               Font.FromFile("C:/Windows/Fonts/Calibri.ttf"),
               4 * Units.cm2m,
               Material.Copy(DefaultIds.materialFont),
               Color.HSV(40.2f, 22.7f, 54.6f));

            if (rightHand.IsJustPinched && !game_over)
            {
                position = rightHand[FingerId.Index, JointId.Tip].position;
                position.y -= 0.2f;
                size += 0.1f;
                //start = true;
            }

                if (size >= 0.1f && size <= 2.0f )
                {
                    sphereModel.Draw(Matrix.TS(Vec3.Zero, size));
                if (start == true)
                    size -= Time.Elapsedf / 20;
                }
                else
                {
                    objects.Clear();
                    if (time1 == "")
                    {
                         time1 = String.Format("{00:00:00} sec", Time.Total);
                    }
                    Text.Add("Geme over! Balloon life time sec.: " + time1, Matrix.TRS(new Vec3(0f, 0, 0), Quat.LookDir(0, 0, 1)), style);
                    game_over = true;
                }
                
        }
        private void DisplayButtonPanel()
        {
            Vec3 headPosition = Input.Head.position;
            Vec3 windowPosition = new Vec3(0.4f, 0f, 0.1f);
            Pose windowPose = new Pose(windowPosition, Quat.LookAt(windowPosition, headPosition));

            UI.WindowBegin("Button Panel", ref windowPose, new Vec2(20f,20f) * Units.cm2m, false);
            
            if (UI.Button("Start"))
            {
               start = true;
            }

            if (UI.Button("Exit"))
            {
                StereoKitApp.Quit();
            }
            UI.WindowEnd();

        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            if (!StereoKitApp.Initialize("StereoKitProject_ballon", Runtime.MixedReality))
                Environment.Exit(1);
            ObjectSet ObjectBlowing = new ObjectSet();
      
            while (StereoKitApp.Step(() =>
            {   
                ObjectBlowing.Update();
            }));

            StereoKitApp.Shutdown();
        }
    }
}
