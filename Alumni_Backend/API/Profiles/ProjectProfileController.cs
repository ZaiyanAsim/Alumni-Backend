using Alumni_Portal.FileUploads.DTO;
using Alumni_Portal.Profiles.DTO;
using Alumni_Portal.Profiles.Services;
using Microsoft.AspNetCore.Mvc;

[Route("api/Admin/profile-project")]
[ApiController]
public class ProfileProjectController : ControllerBase
{
    private readonly ProjectProfileReadService _readService;
    private readonly ProjectProfileUpdateService _updateService;
    private readonly ProjectRequestService _requestService;

    public ProfileProjectController(
        ProjectProfileReadService readService,
        ProjectProfileUpdateService updateService,
        ProjectRequestService requestService)
    {
        _readService = readService;
        _updateService = updateService;
        _requestService = requestService;
    }

    // ── Full profile ──────────────────────────────────────────────────────────

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProjectProfile(int id, CancellationToken ct)
    {
        var profile = await _readService.GetFullProfileAsync(id, ct);
        return Ok(new { data = profile });
    }

    // ── Individual search ─────────────────────────────────────────────────────

    [HttpGet("individuals/search")]
    public async Task<IActionResult> SearchIndividuals([FromQuery] string q, [FromQuery] string? role, CancellationToken ct)
    {
        var results = await _readService.SearchIndividualsAsync(q, role, ct);
        return Ok(new { data = results });
    }

    // ── Description ───────────────────────────────────────────────────────────

    [HttpPatch("{projectId}/description")]
    public async Task<IActionResult> UpdateDescription(int projectId, [FromBody] UpdateDescriptionRequest body)
    {
        await _updateService.UpdateDescriptionAsync(projectId, body.Project_Description);
        return Ok();
    }

    // ── Members ───────────────────────────────────────────────────────────────

    [HttpPost("{projectId}/members/byid")]
    public async Task<IActionResult> AddMember(int projectId, [FromBody] AddMemberRequest body)
    {
        var mapId = await _updateService.AddMemberAsync(projectId, body.IndividualId, body.Role);
        return Ok(new { data = mapId });
    }

    [HttpDelete("members/{mapId}")]
    public async Task<IActionResult> RemoveMember(int mapId)
    {
        await _updateService.RemoveMemberAsync(mapId);
        return Ok();
    }

    // ── Sponsor ────────────────────────────────────────────────────────────────

    [HttpPost("{projectId}/sponsor")]
    public async Task<IActionResult> AddSponsor(int projectId, [FromBody] AddSponsorRequest body)
    {
        var mapId = await _updateService.AddMemberAsync(projectId, body.IndividualId, "Sponsor");
        return Ok(new { data = mapId });
    }

    [HttpDelete("sponsor/{mapId}")]
    public async Task<IActionResult> RemoveSponsor(int mapId)
    {
        await _updateService.RemoveMemberAsync(mapId);
        return Ok();
    }

    // ── Tech Stack ─────────────────────────────────────────────────────────────

    [HttpPost("{projectId}/tech-stack")]
    public async Task<IActionResult> AddTechStack(int projectId, [FromBody] AddTechStackRequest body)
    {
        var stackId = await _updateService.AddTechStackAsync(projectId, body.Technology_Value, body.Layer_Value);
        return Ok(new { data = stackId });
    }

    [HttpDelete("tech-stack/{stackId}")]
    public async Task<IActionResult> RemoveTechStack(int stackId)
    {
        await _updateService.RemoveTechStackAsync(stackId);
        return Ok();
    }

    // ── Methodologies ─────────────────────────────────────────────────────────

    [HttpPost("{projectId}/methodologies")]
    public async Task<IActionResult> AddMethodology(int projectId, [FromBody] AddMethodologyRequest body)
    {
        var methodologyId = await _updateService.AddMethodologyAsync(projectId, body.MethodologyValue);
        return Ok(new { data = methodologyId });
    }

    [HttpDelete("methodologies/{methodologyId}")]
    public async Task<IActionResult> RemoveMethodology(int methodologyId)
    {
        await _updateService.RemoveMethodologyAsync(methodologyId);
        return Ok();
    }

    // ── Attachments ───────────────────────────────────────────────────────────

    [HttpPost("{projectId}/attachments/upload")]
    public async Task<IActionResult> UploadAttachment(int projectId, [FromForm] DocumentUploadRequestDTO document)
    {
        var fileUrl = await _updateService.AddProjectAttachment(projectId, document);
        return Ok(new { data = fileUrl });
    }

    [HttpPost("{projectId}/attachments/link")]
    public async Task<IActionResult> AddAttachmentLink(int projectId, [FromBody] AddAttachmentLinkRequest body)
    {
        await _updateService.AddAttachmentLinkAsync(projectId, body.Title, body.Url, body.Description);
        return Ok();
    }

    [HttpDelete("attachments/{attachmentId}")]
    public async Task<IActionResult> DeleteProjectAttachment(int attachmentId)
    {
        await _updateService.DeleteProjectAttachment(attachmentId);
        return Ok();
    }

    // ── Results ───────────────────────────────────────────────────────────────

    [HttpPost("{projectId}/results")]
    public async Task<IActionResult> AddProjectResult(int projectId, [FromForm] ProjectResultsDTO dto)
    {
        await _updateService.AddProjectResult(projectId, dto);
        return Ok();
    }

    [HttpPut("{projectId}/results/{resultId}")]
    public async Task<IActionResult> UpdateProjectResult(int projectId, int resultId, [FromForm] ProjectResultsDTO dto)
    {
        await _updateService.UpdateProjectResult(projectId, resultId, dto);
        return Ok();
    }

    [HttpDelete("results/{resultId}")]
    public async Task<IActionResult> DeleteProjectResult(int resultId)
    {
        await _updateService.DeleteProjectResult(resultId);
        return Ok();
    }

    // ── Deliverables ──────────────────────────────────────────────────────────

    [HttpPost("{projectId}/deliverables")]
    public async Task<IActionResult> AddProjectDeliverable(int projectId, [FromBody] ProjectDeliverablesDTO dto)
    {
        await _updateService.AddProjectDeliverable(projectId, dto);
        return Ok();
    }

    [HttpPut("{projectId}/deliverables/{deliverableId}")]
    public async Task<IActionResult> UpdateProjectDeliverable(int projectId, int deliverableId, [FromBody] ProjectDeliverablesDTO dto)
    {
        await _updateService.UpdateProjectDeliverable(projectId, deliverableId, dto);
        return Ok();
    }

    [HttpDelete("deliverables/{deliverableId}")]
    public async Task<IActionResult> DeleteProjectDeliverable(int deliverableId)
    {
        await _updateService.DeleteProjectDeliverable(deliverableId);
        return Ok();
    }

    // ── Requests ──────────────────────────────────────────────────────────────

    [HttpGet("{projectId}/requests")]
    public async Task<IActionResult> GetProjectRequests(int projectId, CancellationToken ct)
    {
        var requests = await _requestService.GetRequestsAsync(projectId, ct);
        return Ok(new { data = requests });
    }

    [HttpPatch("requests/{requestId}/accept")]
    public async Task<IActionResult> AcceptProjectRequest(int requestId, CancellationToken ct)
    {
        await _requestService.AcceptRequestAsync(requestId, ct);
        return Ok();
    }

    [HttpDelete("requests/{requestId}")]
    public async Task<IActionResult> RejectProjectRequest(int requestId, CancellationToken ct)
    {
        await _requestService.RejectRequestAsync(requestId, ct);
        return Ok();
    }
}