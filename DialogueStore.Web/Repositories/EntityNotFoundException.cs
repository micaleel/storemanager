using System;

namespace DialogueStore.Web.Repositories {

    public class EntityNotFoundException : Exception {

        public EntityNotFoundException()
            : base("Cannot find entity with given parameters") {
        }

        public EntityNotFoundException(string message)
            : base(message) {
        }
    }
}