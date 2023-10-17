namespace PicoPoints.Web;

public class TestEndpoint : PicoEndpoint<TestRequest, TestResponse>
{
    public override PicoEndpointConfiguration Configure() => new(Method.Get, "/api/test");
    
    protected override Task<PicoEndpointResult<TestResponse>> HandleAsync(TestRequest request)
    {
        return Task.FromResult(Ok(new TestResponse
        {
            Message = $"Hello {request.Name}"
        }));
    }
}

public class TestRequest
{
    public string Name { get; set; }
}
public class TestResponse
{
    public string Message { get; set; }
}
