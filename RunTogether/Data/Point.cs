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
        [NotMapped]
        public Vector2 Coordinate
        {
            get
            {
                return new Vector2(this.X, this.Y);
            }

            set
            {
                this.X = value.X;
                this.Y = value.Y;
            }
        }

        [Required]
        public float X { get; private set; }
        [Required]
        public float Y { get; private set; }

        public int StageId { get; set; }

        public Stage Stage { get; set; }
        public Point(float x, float y) 
        {
            this.Coordinate = new Vector2( x, y);
        }

        protected Point()
        {

        }

    }

    
    [ComplexType]
    public class Coordinate
    {
        public int CoordinateId { get; set; }

        [Column("X")]
        public float X { get; set; }

        [Column("Y")]
        public float Y { get; set; }

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
 