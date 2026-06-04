import { AlertTriangle, X } from 'lucide-react';
import { Button } from '../ui/button';

interface Props {
    mensaje: string;
    onCerrar?: () => void;
}

const ErrorAlert = ({ mensaje, onCerrar }: Props) => {
    return (
        <>
            <div className="flex items-start gap-3 bg-red-300 border border-red-200 text-red-800 px-4 py-3 rounded-lg mb-4">
                <AlertTriangle size={18} className="mt-0.5 shrink-0" />
                <span className="flex-1 text-sm">{mensaje}</span>
                {onCerrar && (
                    <Button
                        onClick={onCerrar}
                        className="shrink-0 text-red-500 hover:text-red-700">
                        <X size={16} />
                    </Button>
                )}
            </div>
        </>
    );
};

export default ErrorAlert;