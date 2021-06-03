using Autofac;
using MediatR;
using Ordering.Application.Commands.OrderCreate;
using Ordering.Application.Commands.OrderDelivered;
using Ordering.Application.Commands.OrderPaymentReject;
using Ordering.Application.Commands.OrderPaymentSuccess;
using Ordering.Application.Commands.OrderShipped;
using Ordering.Application.Commands.OrderStatus;
using System.Linq;
using System.Reflection;

namespace Ordering.Application.AutofacModules
{
    public class MediatorModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                            .AsImplementedInterfaces();

            #region Commands
            builder.RegisterAssemblyTypes(typeof(OrderStatusCommand).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<,>));
            builder.RegisterAssemblyTypes(typeof(OrderCreateCommand).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<,>));
            builder.RegisterAssemblyTypes(typeof(OrderDeliveredCommand).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<,>));
            builder.RegisterAssemblyTypes(typeof(OrderPaymentRejectCommand).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<,>));
            builder.RegisterAssemblyTypes(typeof(OrderPaymentSuccessCommand).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<,>));
            builder.RegisterAssemblyTypes(typeof(OrderShippedCommand).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<,>));
            #endregion
        }
    }
}
