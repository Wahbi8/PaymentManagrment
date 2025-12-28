using PaymentManagement.Domain;
using PaymentManagement.Infrastructure;

namespace PaymentManagement.Application.Services
{
    public class CustomerServices
    {
        private readonly CustomerRepository _customerRepository;
        public CustomerServices(CustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public async Task<Customer> GetCustomerById(Guid id)
        {
            if (id == Guid.Empty)
                throw new BusinessException("Customer id cannot be empty");
            var customer = await _customerRepository.GetCustomerById(id);
            if (customer == null)
                throw new BusinessException("Customer not found");
            return customer;
        }
        public async Task<List<Customer>> GetAllCustomers()
        {
            var customers = await _customerRepository.GetAllCustomers();
            if (customers == null || !customers.Any())
                throw new BusinessException("No customers found");
            return customers;
        }
        public async Task AddCustomer(Customer customer)
        {
            if (customer == null)
                throw new BusinessException($"{nameof(Customer)} cannot be empty");
            customer.Id = Guid.NewGuid();
            await _customerRepository.AddCustomer(customer);
        }
        public async Task UpdateCustomer(Customer customer)
        {
            if (customer == null)
                throw new BusinessException($"{nameof(Customer)} cannot be empty");
            await _customerRepository.UpdateCustomer(customer);
        }
        public async Task DeleteCustomer(Guid id)
        {
            if (id == Guid.Empty)
                throw new BusinessException("Customer id cannot be empty");
            await _customerRepository.DeleteCustomer(id);
        }
    }
}
