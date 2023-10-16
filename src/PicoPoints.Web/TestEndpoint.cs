namespace PicoPoints.Web;

public class TestEndpoint : PicoEndpoint<TestRequest, TestResponse>
{
    public override PicoEndpointConfiguration Configure() => new(Method.Get, "/api/test");
}

public class TestRequest {}
public class TestResponse {}
