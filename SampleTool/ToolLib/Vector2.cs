using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolLib
{
    public class Vector2
    {
        public double X;
        public double Y;
        //构造函数
        public Vector2(double x, double y)
        {
            X = x;
            Y = y;
        }
        //获取向量的长度
        public double GetLength()
        {
            double Length = Math.Sqrt(X * X + Y * Y);
            return Length;
        }
        //重载运算符==
        public static bool operator ==(Vector2 a, Vector2 b)
        {
            return ((a.X == b.X) && (a.Y == b.Y));
        }
        //重载运算符!=
        public static bool operator !=(Vector2 a, Vector2 b)
        {
            return !(a == b);
        }
        //重载运算符>：以向量长度判断是否为真
        public static bool operator >(Vector2 a, Vector2 b)
        {
            return a.GetLength() > b.GetLength();
        }
        //重载运算符<
        public static bool operator <(Vector2 a, Vector2 b)
        {
            return a.GetLength() < b.GetLength();
        }
        //重载运算符>=
        public static bool operator >=(Vector2 a, Vector2 b)
        {
            return (a == b) || (a > b);
        }
        //重载运算符<=
        public static bool operator <=(Vector2 a, Vector2 b)
        {
            return (a == b) || (a < b);
        }



        //重载运算符+
        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X + b.X, a.Y + b.Y);
        }

        //重载运算符-
        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X - b.X, a.Y - b.Y);
        }

        public Vector2 Normalized()
        {
            double dis = Math.Sqrt(X * X + Y * Y);
            return new Vector2(X / dis, Y / dis);
        }

    }

}
