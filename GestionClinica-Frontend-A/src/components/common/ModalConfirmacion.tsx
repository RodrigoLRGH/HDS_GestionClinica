import { AlertTriangle, X } from 'lucide-react';
import { Button } from '@/components/ui/button';

interface Props {
    titulo: string;
    mensaje: string;
    onConfirmar: () => void;
    onCancelar: () => void;
}

const ModalConfirmacion = ({ titulo, mensaje, onConfirmar, onCancelar }: Props) => {
    return (
        <>
            <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50">
                <div className="bg-white rounded-xl shadow-xl p-6 max-w-sm w-full mx-4">
                    <div className="flex justify-between items-center mb-4">
                        <div className="flex items-center gap-2 text-red-600">
                            <AlertTriangle size={22} />
                            <h3 className="text-lg font-bold">{titulo}</h3>
                        </div>
                        <button
                            onClick={onCancelar}
                            className="text-gray-400 hover:text-gray-600">
                            <X size={20} />
                        </button>
                    </div>

                    <p className="text-gray-600 text-sm mb-6">{mensaje}</p>

                    <div className="flex justify-end gap-2">
                        <Button
                            onClick={onCancelar}
                            className="px-4 py-2 rounded-lg bg-gray-100 text-gray-700 hover:bg-gray-200">
                            No, volver
                        </Button>
                        <Button
                            onClick={onConfirmar}
                            className="px-4 py-2 rounded-lg bg-red-600 text-white hover:bg-red-700">
                            Sí, cancelar
                        </Button>
                    </div>
                </div>
            </div>
        </>
    );
};

export default ModalConfirmacion;