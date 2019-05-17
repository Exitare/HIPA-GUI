using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIPA.Classes
{
    partial class Exceptions
    {
        public class DataMatrixEmpty : Exception
        {
            public DataMatrixEmpty()
            {
            }

            public DataMatrixEmpty(string message) : base(message)
            {
            }

            public DataMatrixEmpty(string message, Exception inner) : base(message, inner)
            {
            }
        }


        public class NoNormalizationMethodFound : Exception
        {
            public NoNormalizationMethodFound()
            {
            }

            public NoNormalizationMethodFound(string message) : base(message)
            {
            }

            public NoNormalizationMethodFound(string message, Exception inner) : base(message, inner)
            {
            }
        }


        public class NoTimeFrameMaximumDetected : Exception
        {
            public NoTimeFrameMaximumDetected()
            {
            }

            public NoTimeFrameMaximumDetected(string message) : base(message)
            {
            }

            public NoTimeFrameMaximumDetected(string message, Exception inner) : base(message, inner)
            {
            }
        }


        public class CouldNotCalculateThreshold : Exception
        {
            public CouldNotCalculateThreshold()
            {
            }

            public CouldNotCalculateThreshold(string message) : base(message)
            {
            }

            public CouldNotCalculateThreshold(string message, Exception inner) : base(message, inner)
            {
            }
        }

        public class CouldNotCalculateBaselineMean : Exception
        {
            public CouldNotCalculateBaselineMean()
            {
            }

            public CouldNotCalculateBaselineMean(string message) : base(message)
            {
            }

            public CouldNotCalculateBaselineMean(string message, Exception inner) : base(message, inner)
            {
            }
        }

        public class CouldNotCountHighIntensityPeaksPerMinute : Exception
        {
            public CouldNotCountHighIntensityPeaksPerMinute()
            {
            }

            public CouldNotCountHighIntensityPeaksPerMinute(string message) : base(message)
            {
            }

            public CouldNotCountHighIntensityPeaksPerMinute(string message, Exception inner) : base(message, inner)
            {
            }
        }


        public class InvalidCharacterFound : Exception
        {
            public InvalidCharacterFound()
            {
            }

            public InvalidCharacterFound(string message) : base(message)
            {
            }

            public InvalidCharacterFound(string message, Exception inner) : base(message, inner)
            {
            }
        }


        public class InvalidSeperator : Exception
        {
            public InvalidSeperator()
            {
            }

            public InvalidSeperator(string message) : base(message)
            {
            }

            public InvalidSeperator(string message, Exception inner) : base(message, inner)
            {
            }
        }

        public class DataCheckNotPassed : Exception
        {
            public DataCheckNotPassed()
            {
            }

            public DataCheckNotPassed(string message) : base(message)
            {
            }

            public DataCheckNotPassed(string message, Exception inner) : base(message, inner)
            {
            }
        }
    }
}
