using System.Threading.Tasks;
using Aigang.Platform.Contracts.Errors;
using Aigang.Platform.Contracts.Insurance.Android;
using Aigang.Platform.Contracts.Insurance.Requests;
using Aigang.Platform.Contracts.Insurance.Responses;
using Aigang.Platform.Domain.Insurance;
using Aigang.Platform.Handlers;
using Aigang.Platform.Handlers.Insurance.Android;
using Aigang.Platform.Handlers.Utils;
using log4net;
using Microsoft.AspNetCore.Mvc;

namespace Aigang.Platform.API.Controllers
{
	[Route("api/insurance")]
    public class InsuranceController : ControllerBase
    {
	    private static readonly ILog _logger = LogManager.GetLogger(typeof(InsuranceController));
	    private readonly CreatePolicyHandlerResolver _createPolicyHandlerResolver;
	    private readonly VerifyAndroidPolicyHandler _verifyAndroidPolicyHandler;
	    private readonly PairDeviceHandler _pairDeviceHandler;
	    private readonly GetPolicyTransactionsHandler _getPolicyTransactionsHandler;

	    public InsuranceController()
	    {
		    _createPolicyHandlerResolver = new CreatePolicyHandlerResolver(_logger);
		    _pairDeviceHandler = new PairDeviceHandler(_logger);
     	    _verifyAndroidPolicyHandler = new VerifyAndroidPolicyHandler(_logger);
            _getPolicyTransactionsHandler = new GetPolicyTransactionsHandler(_logger);

	    }
	
	    /// <summary>
	    /// Pair device with client browser
	    /// </summary>
	    /// <returns>Task id</returns>
	    [HttpPost("pair")]
	    [ProducesResponseType(typeof(PairDeviceResponse), 200)]
	    [ProducesResponseType(typeof(ValidationFailedResponse), 400)]
	    [ProducesResponseType(typeof(InternalServerErrorResponse), 500)]
	    public async Task<IActionResult> PairDevice([FromBody]PairDeviceRequest request)
	    {
		    var response = await _pairDeviceHandler.HandleAsync(request);
		    var result = MakeActionResult(response);
		    return result;
	    }
	  
	    /// <summary>
	    /// Create policy
	    /// </summary>	
	    /// <returns>PolicyDto</returns>
	    [HttpPost("policy")]
	    [ProducesResponseType(typeof(CreatePolicyResponse), 200)]
	    [ProducesResponseType(typeof(ValidationFailedResponse), 400)]
	    [ProducesResponseType(typeof(InternalServerErrorResponse), 500)]
	    public async Task<IActionResult> CreatePolicy([FromBody]CreatePolicyRequest request)
	    {
		    var createPolicyHandler = _createPolicyHandlerResolver.GetHandler((ProductType)request.ProductTypeId);

		    ActionResult result;
		    
		    if (createPolicyHandler != null)
		    {
			    var response = await createPolicyHandler.HandleAsync(request);
			    result = MakeActionResult(response);
		    }
		    else
		    {
			    var response = new CreatePolicyResponse();           
			    response.Error = new ValidationFailedResponse("ProductTypeId is not supported");
			    result = MakeActionResult(response);
		    }
		    
		    return result;
	    }
	    
	    
	    // PUT api/insure/verifypolicyclaim/id
	    /// <summary> Verify Is Policy Valid For Claim </summary>
	    /// <returns> Getting Is Policy Valid For Claim </returns>
	    [HttpPut("verifypolicyclaim")]
	    [ProducesResponseType(typeof(VerifyPolicyClaimResponse), 200)]
	    [ProducesResponseType(typeof(ValidationFailedResponse), 400)]
	    [ProducesResponseType(typeof(InternalServerErrorResponse), 500)]
	    public async Task<IActionResult> VerifyPolicyClaim([FromBody]VerifyPolicyClaimRequest request)
	    {
		    var response = await _verifyAndroidPolicyHandler.HandleAsync(request);
		    var result = MakeActionResult(response);
		    return result;
	    }
	    
	    // GET api/insure/policytransactions/id
	    /// <summary> Get Policy Transactions </summary>
	    /// <returns> Getting Policy Transactions </returns>
	    [HttpGet("policytransactions/{policyId}")]
	    [ProducesResponseType(typeof(GetPolicyTransactionsResponse), 200)]
	    [ProducesResponseType(typeof(ValidationFailedResponse), 400)]
	    [ProducesResponseType(typeof(InternalServerErrorResponse), 500)]
	    public async Task<IActionResult> GetPolicyTransactions(string policyId)
	    {
		    var response = await _getPolicyTransactionsHandler.HandleAsync(new BasePolicyRequest{ PolicyId =  policyId});
		    var result = MakeActionResult(response);
		    return result;
	    }
	    
    }
}
