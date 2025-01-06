using static Zenex.Master.Models.Master;

namespace Zenex.Master.IRespository
{
    public interface ILocationRepository
    {
        CBPLocation GetLocationByPincode(string Pincode);
        List<StateDetails> GetStateDetails();
        List<MyArray> GetLocation(string Pincode);
    }
}
