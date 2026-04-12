
using Alumni_Portal.Profiles.DTO;
using Alumni_Portal.Profiles.Repositories;
using Microsoft.Identity.Client;

namespace Alumni_Portal.Profiles.Services
{
    public class ProjectProfileService
    {
        private readonly ProjectReadRepo _repo;

        public ProjectProfileService(ProjectReadRepo repo)
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

      
        public async Task<ProjectProfileResponseDTO> GetFullProfileAsync(
            int projectId,
            CancellationToken ct = default)
        {
            var headerTask = GetHeaderAsync(projectId, ct);
            var membersTask = GetMembersAsync(projectId, ct);
            var documentsTask = GetDocumentsAsync(projectId, ct);
            var resultsTask = GetResultsAsync(projectId, ct);
            var deliverablesTask = GetDeliverablesAsync(projectId, ct);

            await Task.WhenAll(
                headerTask,
                membersTask,
                documentsTask,
                resultsTask,
                deliverablesTask);

            return new ProjectProfileResponseDTO
            {
                Header_Data =  headerTask.Result,
                Members =  membersTask.Result ,
                Documents = documentsTask.Result,
                Results =  resultsTask.Result,
                Deliverables =  deliverablesTask.Result,
            };
        }
    }
}