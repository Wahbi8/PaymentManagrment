using PaymentManagement.Domain;

namespace PaymentManagement.Domain.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer> GetCustomerById(Guid id);
        Task<List<Customer>> GetAllCustomers();
        Task AddCustomer(Customer customer);
        Task DeleteCustomer(Guid id);
        Task UpdateCustomer(Customer customer);
    }
}
