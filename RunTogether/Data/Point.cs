using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace RunTogether.Data
{
    public abstract class Point
    {
        [Required]
        public float X { get; set; }
        [Required]
        public float Y { get; set; }

        public int StageId { get; set; }
        
        public Stage? Stage { get; set; } 
        public Point(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        protected Point()
        {

        }

    }

    public class StartPoint : Point
    {
        public int StartPointId { get; set; }

        public StartPoint(float x, float y) : base(x, y)
        {

        }
        protected StartPoint() : base()
        {

        }
    }

    public class EndPoint : Point
    {
        public int EndPointId { get; set; }

        public EndPoint(float x, float y) : base(x, y)
        {

        }

        protected EndPoint() : base()
        {

        }
    }

    public class ThroughPoint : Point
    {
        public int ThroughPointId { get; set; }

        public ThroughPoint(float x, float y) : base(x, y)
        {

        }

        protected ThroughPoint() : base()
        {

        }
    }

}
 