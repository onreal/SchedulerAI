namespace SchedulerApi.Domain.Template.Contracts
{
    public interface ITemplateRepository
    {
        Task AddAsync(Template template);
        Task<Template?> GetByIdAsync(Guid id);
        Task<IEnumerable<Template>> GetAllAsync();
        Task UpdateAsync(Template template);
        Task DeleteAsync(Template template);
    }
}