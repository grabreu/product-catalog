# Mediator dispatch, FluentValidation pipeline behavior, Desfecho for results

Commands and queries are dispatched through `Mediator` (source-generator based; MediatR was ruled out after its move to a dual commercial license). FluentValidation runs as a `Mediator` pipeline behavior (`ValidationBehavior`) ahead of every handler, so validation logic stays out of handler bodies; failures are converted to `Desfecho`'s `Error.Validation` and returned as the handler's own `Result<T>` type through a dynamic cast, since the generic behavior can't know the concrete response type statically.

**Consequences**: that dynamic cast in `ValidationBehavior` is the one non-obvious detail to keep in mind; in exchange, any new handler gets validation for free just by registering a validator for its command/query.
