
namespace Sidewinder.Interfaces.Entities
{
    public class DistributorContext
    {                
        public DistributorConfig Config { get; set; }

        private string myBinariesFolder;
        public string BinariesFolder
        {
            get { return myBinariesFolder; }
            set { myBinariesFolder = SmartLocation.GetLocation(value); }
        }
    }
}