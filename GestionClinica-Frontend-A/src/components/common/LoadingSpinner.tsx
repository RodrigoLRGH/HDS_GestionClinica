import { LucideLoader } from "lucide-react";

const LoadingSpinner = () => {
    return (
        <>
            <div className="flex justify-center items-center py-10">
                <LucideLoader className="animate-spin" />
            </div>
        </>
    )
}

export default LoadingSpinner;