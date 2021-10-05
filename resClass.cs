using System;
using System.Collections.Generic;
using System.Text;

namespace YOLOv4MLNet
{
    public class resClass
    {
        string Label;
        float[] Box;
        float Confidence;

        public resClass(string label, float x1, float x2, float x3, float x4, float conf)
        {
            Label = label;
            Box = new float[4];
            Box[0] = x1;
            Box[1] = x2;
            Box[2] = x3;
            Box[3] = x4;
            Confidence = conf;
        }

        public string label
        {
            get
            {
                return Label;
            }
        }

        public float[] box
        {
            get
            {
                return Box;
            }
        }

        public float confidence
        {
            get
            {
                return Confidence;
            }
        }

        public override string ToString()
        {
            return Label + "\n" + Box[0].ToString() + "\n" + Box[1].ToString() + "\n" + Box[2].ToString() + "\n" + Box[3].ToString() + "\n" + Confidence.ToString();
        }
    }

    public class imageRes
    {
        string ImgName;
        List<resClass> Results;

        public imageRes(string name, List<resClass> r)
        {
            ImgName = name;
            Results = r;
        }

        public string imgName
        {
            get
            {
                return ImgName;
            }
        }

        public List<resClass> results
        {
            get
            {
                return Results;
            }
        }

        public override string ToString()
        {
            string res = ImgName + "\n";
            foreach(var item in Results)
                res = res + item.ToString() + "\n";
            return res;
        }
    }
}
