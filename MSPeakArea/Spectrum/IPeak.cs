using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSPeakArea.Spectrum
{
    public interface IPeak
    {
        double GetMZ();
        double setMZ(double mz);
        double GetIntensity();
        void SetIntensity(double intensity);
    }
}
