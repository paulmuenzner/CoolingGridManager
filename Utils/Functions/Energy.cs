
using CoolingGridManager.Models.Data;

namespace Utility.Functions
{
    public static class Energy
    {
        public static decimal CalculateGridEnergyTransfer(List<GridParameterLog> logs)
        {

            // Initialize the total cooling energy transfer
            decimal totalEnergyTransfer_kWh = 0;

            // Iterate through each GridParameterLog object in the array
            foreach (var log in logs)
            {
                // Calculate time span 
                int timeSpan = Date.TimeSpanInSeconds(log.DateTimeStart, log.DateTimeEnd);

                // Calculate the cooling energy transfer for the current log entry 
                // E_month = (m_dot) * (cp) * (T_in - T_out) * (t) / (3600 * 1000)
                decimal deltaTemp = log.MeanTemperatureIn - log.MeanTemperatureOut; // Temp. difference between flow and return of the medium in cooling water pipe 
                decimal energyTransferGrid_kWh = -1 * log.MassFlowRate * log.SpecificHeatCapacity * deltaTemp * timeSpan / (3600 * 1000);

                // Add the cooling energy transfer to the total
                totalEnergyTransfer_kWh += energyTransferGrid_kWh;
            }

            return Numbers.RoundDecimal(totalEnergyTransfer_kWh, 4);
        }
    }

}
