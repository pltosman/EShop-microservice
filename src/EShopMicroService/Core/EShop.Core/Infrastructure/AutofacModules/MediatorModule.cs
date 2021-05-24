using System;
using System.Reflection;
using Autofac;
using EShop.Core.Application.Commands.RegisterCommands;
using EShop.Core.Application.PipelineBehaviours;
using EShop.Core.Infrastructure.Idempotency;
using FluentValidation;
using MediatR;

namespace EShop.Core.Infrastructure.AutofacModules
{
    public class MediatorModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                            .AsImplementedInterfaces();

            #region Commands
            builder.RegisterAssemblyTypes(typeof(RegisterCustomerCommand).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<,>));
            #endregion

            #region Queries
           // builder.RegisterType<CustomerQueries>().As<ICustomerQueries>().InstancePerLifetimeScope();
            #endregion

            #region Validators
            builder
                .RegisterAssemblyTypes(typeof(MediatorModule).GetTypeInfo().Assembly)
                .Where(x => x.IsClosedTypeOf(typeof(IValidator<>)))
                .AsImplementedInterfaces();
            #endregion

            builder.RegisterType<RequestManager>()
               .As<IRequestManager>()
               .InstancePerLifetimeScope();

            builder.Register<ServiceFactory>(context =>
            {
                var componentContext = context.Resolve<IComponentContext>();
                return t => { object o; return componentContext.TryResolve(t, out o) ? o : null; };
            });

            //Behaviors
            builder.RegisterGeneric(typeof(LoggingBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(ValidatorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        }
    }
}
