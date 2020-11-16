export function isClassOrSubclass(instance, cls) {
    if (instance === null)
        return false;

    return instance instanceof cls ||
        (instance !== undefined && instance.prototype instanceof cls);
}