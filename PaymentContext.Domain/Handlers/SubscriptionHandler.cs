using System;
using Flunt.Notifications;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Repositories;
using PaymentContext.Domain.Services;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Shared.Commands;
using PaymentContext.Shared.Handlers;

namespace PaymentContext.Domain.Handlers
{
    public class SubscriptionHandler : Notifiable, IHandler<CreateBoletoSubscriptionCommand>, 
                                                   IHandler<CreatePayPalSubscriptionCommand>,
                                                   IHandler<CreateCreditCardSubscriptionCommand>
    {
        private readonly IStudentRepository _repository;
        private readonly IEmailService _emailService;
        public SubscriptionHandler(IStudentRepository repository, IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }

        public ICommandResult Handle(CreateBoletoSubscriptionCommand command)
        {
            // Fail fast validation
            command.Validate();
            if (command.Invalid)
            {
                AddNotifications(command);
                return new CommandResult(false, "Não foi possível realizar sua assinatura");
            }

            // Verificar se documento está cadastrado
            if (_repository.DocumentExists(command.Document))
                AddNotification("Document", "Este CPF já está em uso");

            // Verificar se email está cadastrado
            if (_repository.DocumentExists(command.Email))
                AddNotification("Email", "Este e-mail já está em uso");

            // Gerar os VOs
            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.Document, EDocumentType.CPF);
            var email = new Email(command.Email);
            var address = new Address(command.Street, command.Number, command.Neighborhood,
                                        command.City, command.State, command.Country, command.ZipCode);

            // Gerar entidades
            var student = new Student(name, document, email);
            var subscription = new Subscription(DateTime.Now.AddMonths(1));
            var payment = new BoletoPayment(command.BarCode, command.BoletoNumber, command.PaidDate,
                                            command.ExpireDate, command.Total, command.TotalPaid, command.Payer,
                                            new Document(command.PayerDocument, command.PayerDocumentType), address, email);

            return CreateSubscription(payment, subscription, student);
        }

        public ICommandResult Handle(CreatePayPalSubscriptionCommand command)
        {
            // Fail fast validation
            command.Validate();
            if (command.Invalid)
            {
                AddNotifications(command);
                return new CommandResult(false, "Não foi possível realizar sua assinatura");
            }

            // Verificar se documento está cadastrado
            if (_repository.DocumentExists(command.Document))
                AddNotification("Document", "Este CPF já está em uso");

            // Verificar se email está cadastrado
            if (_repository.DocumentExists(command.Email))
                AddNotification("Email", "Este e-mail já está em uso");

            // Gerar os VOs
            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.Document, EDocumentType.CPF);
            var email = new Email(command.Email);
            var address = new Address(command.Street, command.Number, command.Neighborhood,
                                        command.City, command.State, command.Country, command.ZipCode);

            // Gerar entidades
            var student = new Student(name, document, email);
            var subscription = new Subscription(null);
            var payment = new PayPalPayment(command.TransactionCode, command.PaidDate,
                                            command.ExpireDate, command.Total, command.TotalPaid, command.Payer,
                                            new Document(command.PayerDocument, command.PayerDocumentType), address, email);

            return CreateSubscription(payment, subscription, student);
        }

        public ICommandResult Handle(CreateCreditCardSubscriptionCommand command)
        {
            // Fail fast validation
            command.Validate();
            if (command.Invalid)
            {
                AddNotifications(command);
                return new CommandResult(false, "Não foi possível realizar sua assinatura");
            }

            // Verificar se documento está cadastrado
            if (_repository.DocumentExists(command.Document))
                AddNotification("Document", "Este CPF já está em uso");

            // Verificar se email está cadastrado
            if (_repository.DocumentExists(command.Email))
                AddNotification("Email", "Este e-mail já está em uso");

            // Gerar os VOs
            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.Document, EDocumentType.CPF);
            var email = new Email(command.Email);
            var address = new Address(command.Street, command.Number, command.Neighborhood,
                                        command.City, command.State, command.Country, command.ZipCode);

            // Gerar entidades
            var student = new Student(name, document, email);
            var subscription = new Subscription(DateTime.Now.AddMonths(1));
            var payment = new CreditCardPayment(command.CardNumber, command.CardHolderName, command.LastTransactionNumber,
                                                command.PaidDate, command.ExpireDate, command.Total, command.TotalPaid, command.Payer,
                                                new Document(command.PayerDocument, command.PayerDocumentType), address, email);

            return CreateSubscription(payment, subscription, student);
        }

        private CommandResult CreateSubscription(Payment payment, Subscription subscription, Student student){
            // Relacionamentos
            subscription.AddPayment(payment);
            student.AddSubscription(subscription);

            // Agrupar validacoes
            AddNotifications(student.Name, student.Document, student.Email, payment.Address, student, subscription, payment);

            // Salvar informações
            _repository.CreateSubscription(student);

            // Enviar email de boas vindas
            _emailService.Send(student.Name.ToString(), student.Email.Address,
                                "Bem vindo ao balta.io", "Sua assinatura foi criada");

            // Retornar informaçoes
            return new CommandResult(true, "Assinatura realizada com sucesso.");
        }
    }
}