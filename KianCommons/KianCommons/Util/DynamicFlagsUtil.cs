namespace KianCommons {
    using System;

    public static class DynamicFlagsUtil {
        public readonly static ulong[] EMPTY_FLAGS = new ulong[0];
        public readonly static string[] EMPTY_TAGS = new string[0];
        public readonly static
            DynamicFlags<NetInfo> NONE = new DynamicFlags<NetInfo>(EMPTY_FLAGS);

        public static bool CheckFlags(this DynamicFlags<NetInfo> flags, DynamicFlags<NetInfo> required, DynamicFlags<NetInfo> forbidden) =>
            DynamicFlags<NetInfo>.CheckAll(flags, required: required, forbidden: forbidden);

        public static bool IsAnyFlagSet(this DynamicFlags<NetInfo> flags, DynamicFlags<NetInfo> flags2) =>
            !DynamicFlags<NetInfo>.CheckAny(flags, required: new DynamicFlags<NetInfo>(EMPTY_FLAGS), forbidden: flags2);

        public static bool CheckFlags(this DynamicFlags2 flags, DynamicFlags2 required, DynamicFlags2 forbidden) =>
            DynamicFlags2.CheckFlags(flags, required: required, forbidden: forbidden);

        public static bool IsAnyFlagSet(this DynamicFlags2 flags, DynamicFlags2 flags2) =>
            !DynamicFlags2.IsAnyFlagSet(flags, flags2);
    }
}
