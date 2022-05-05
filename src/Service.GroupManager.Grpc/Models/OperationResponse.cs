using System.Runtime.Serialization;

namespace Service.GroupManager.Grpc.Models;

[DataContract]
public class OperationResponse
{
    public OperationResponse()
    {
        IsSuccess = true;
    }

    public OperationResponse(string errorMessage)
    {
        IsSuccess = false;
        ErrorMessage = errorMessage;
    }

    [DataMember(Order = 1)] public bool IsSuccess { get; set; }
    [DataMember(Order = 2)] public string ErrorMessage { get; set; }
}