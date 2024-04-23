
using CoolingGridManager.Models.Data;

namespace Utility.Functions
{
    public static class Energy
    {
        public static decimal CaluclateGridConsumption(List<GridParameterLog> logs)
        {

            // Initialize the total cooling energy transfer
            decimal totalEnergyTransfer = 0;

            // Iterate through each GridParameterLog object in the array
            foreach (var log in logs)
            {
                // Calculate time span 
                int timeSpan = Date.TimeSpanInSeconds(log.DateTimeStart, log.DateTimeEnd);

                // Calculate the cooling energy transfer for the current log entry 
                // Q = (m_dot) * (cp) * (T_in - T_out) * (t)
                decimal deltaTemp = log.MeanTemperatureIn - log.MeanTemperatureOut; // Temp. difference between flow and return of the medium in cooling water pipe 
                decimal energyTransfer = -1 * log.MassFlowRate * log.SpecificHeatCapacity * deltaTemp * timeSpan;

                // Add the cooling energy transfer to the total
                totalEnergyTransfer += energyTransfer;
            }

            decimal result = Numbers.RoundDecimal(totalEnergyTransfer, 4);
            return result;

        }
    }

}
