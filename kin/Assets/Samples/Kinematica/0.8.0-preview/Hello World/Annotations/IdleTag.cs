using System;
using Unity.Kinematica;
using Unity.Kinematica.Editor;

namespace HelloWorld
{

    [Trait]
    public struct IdleTrait
    {
        public static IdleTrait Trait => new IdleTrait();
    }

    [Serializable]
    [Tag("IdleTag1", "#4850d2")]
    public struct IdleTag : Payload<IdleTrait>
    {
        public static IdleTag CreateDefaultTag()
        {
            return new IdleTag();
        }

        public IdleTrait Build(PayloadBuilder builder)
        {
            return IdleTrait.Trait;
        }
    }
}

