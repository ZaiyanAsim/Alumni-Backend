
using Alumni_Portal.Profiles.DTO;
using Alumni_Portal.Profiles.Repositories;
using Microsoft.Identity.Client;

namespace Alumni_Portal.Profiles.Services
{
    public class ProjectProfileReadService
    {
        private readonly ProjectReadRepo _repo;

        public ProjectProfileReadService(ProjectReadRepo repo)
        {
            _repo = repo;
        }

      

        public async Task<MetaDataDTO?> GetHeaderAsync(
            int projectId,
            CancellationToken ct = default)
        {
            var result = await _repo.GetProjectHeaderAsync(projectId, ct);
            return result as MetaDataDTO;
        }

        public Task<List<MemberDTO>> GetMembersAsync(
            int projectId,
            CancellationToken ct = default)
            => _repo.GetMembers(projectId, ct);

        public Task<List<ProjectDocumentDto>> GetDocumentsAsync(
            int projectId,
            CancellationToken ct = default)
            => _repo.GetDocuments(projectId);   

        public Task<List<ProjectResultsDTO>> GetResultsAsync(
            int projectId,
            CancellationToken ct = default)
            => _repo.GetResultsDTOs(projectId);

        public Task<List<ProjectDeliverablesDTO>> GetDeliverablesAsync(
            int projectId,
            CancellationToken ct = default)
            => _repo.GetDeliverablesDTOs(projectId);

      
        public Task<List<TechStackDTO>> GetTechStackAsync(
            int projectId,
            CancellationToken ct = default)
            => _repo.GetTechStackAsync(projectId);

        public async Task<ProjectProfileResponseDTO> GetFullProfileAsync(
            int projectId,
            CancellationToken ct = default)
        {
            var header       = await GetHeaderAsync(projectId, ct);
            var members      = await GetMembersAsync(projectId, ct);
            var documents    = await GetDocumentsAsync(projectId, ct);
            var results      = await GetResultsAsync(projectId, ct);
            var deliverables = await GetDeliverablesAsync(projectId, ct);
            var techStack    = await GetTechStackAsync(projectId, ct);

            return new ProjectProfileResponseDTO
            {
                Header_Data  = header,
                Members      = members,
                Documents    = documents,
                Results      = results,
                Deliverables = deliverables,
                TechStack    = techStack,
            };
        }
    }
}