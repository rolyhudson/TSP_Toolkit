using BH.oM.Analytical.Results;
using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.TSP
{
    public class SolarResult : IObjectIdResult, ICasedResult, ITimeStepResult, IImmutable
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        public virtual IComparable ObjectId { get; }

        public virtual IComparable ResultCase { get; }

        public virtual double TimeStep { get; }

        public virtual double SolarAccess { get; }

        public virtual Unit Unit { get; }

        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public SolarResult(IComparable objectId, IComparable resultCase, double timeStep, double solarAccess, Unit unit)
        {
            ObjectId = objectId;
            ResultCase = resultCase;
            TimeStep = timeStep;
            SolarAccess = solarAccess;
            Unit = unit;
        }

        /***************************************************/
        /**** IComparable Interface                     ****/
        /***************************************************/

        public int CompareTo(IResult other)
        {
            SolarResult otherRes = other as SolarResult;

            if (otherRes == null)
                return this.GetType().Name.CompareTo(other.GetType().Name);

            int n = this.ObjectId.CompareTo(otherRes.ObjectId);
            if (n == 0)
            {
                int l = this.ResultCase.CompareTo(otherRes.ResultCase);
                return l == 0 ? this.TimeStep.CompareTo(otherRes.TimeStep) : l;
            }
            else
            {
                return n;
            }

        }

        /***************************************************/
    }
}
