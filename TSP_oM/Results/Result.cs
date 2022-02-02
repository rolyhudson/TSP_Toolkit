using BH.oM.Analytical.Results;
using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.TSP
{
    public class Result : IObjectIdResult, ICasedResult, ITimeStepResult, IImmutable
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        public virtual IComparable ObjectId { get; }

        public virtual IComparable ResultCase { get; }

        public virtual double TimeStep { get; }

        public virtual Field Field { get; }

        public virtual List<Bar> Bars { get; }

        public virtual List<SolarResult> SolarResults { get; }


        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public Result(IComparable objectId, IComparable resultCase, double timeStep, Field field, List<Bar> bars, List<SolarResult> solarResults)
        {
            ObjectId = objectId;
            ResultCase = resultCase;
            TimeStep = timeStep;
            Field = field;
            Bars = bars;
            SolarResults = solarResults;
        }

        /***************************************************/
        /**** IComparable Interface                     ****/
        /***************************************************/

        public int CompareTo(IResult other)
        {
            Result otherRes = other as Result;

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
