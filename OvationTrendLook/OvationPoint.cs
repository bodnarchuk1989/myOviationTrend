using System;

namespace OvationTrendLook
{
    public class OvationPoint
    {
        public OvationPoint ()
        {
            PoinAlias="";
            PoinName = "";
        }
        OvationPoint(string _poinName, string _pointAlias)
        {
            PoinName=_poinName;
            PoinAlias=_pointAlias;
        }

        public string PoinName {
            get;
            set;
        }

        public string PoinAlias {
            get;
            set;
        }

        public string IO_location {
            get;
            set;
        }

        public string DESCRIPTION {
            get;
            set;
        }
        public string IO_chanel {
            get;
            set;
        }

        public float maxValue {
            set;
            get;
        }

        public float minValue {
            set;
            get;
        }
    }
}

