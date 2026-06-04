interface Props {
    titulo: string;
    mensaje: string;
    onConfirmar: () => void;
    onCancelar: () => void;
}

const ModalConfirmacion = ({ titulo, mensaje, onConfirmar, onCancelar }: Props) => {
    return (
        <>
            <div
                className="modal d-block"
                style={{ backgroundColor: 'rgba(0,0,0,0.5)' }}
                tabIndex={-1}>
                <div className="modal-dialog modal-dialog-centered">
                    <div className="modal-content rounded-3 shadow">
                        <div className="modal-header border-bottom-0 pb-0">
                            <div className="d-flex align-items-center gap-2 text-danger">
                                <i className="bi bi-exclamation-triangle-fill fs-5"></i>
                                <h5 className="modal-title fw-bold mb-0">{titulo}</h5>
                            </div>
                            <button
                                onClick={onCancelar}
                                className="btn-close"
                                aria-label="Cerrar"
                            />
                        </div>
                        <div className="modal-body">
                            <p className="text-muted small mb-0">{mensaje}</p>
                        </div>
                        <div className="modal-footer border-top-0 pt-0">
                            <button
                                onClick={onCancelar}
                                className="btn btn-secondary">
                                No, volver
                            </button>
                            <button
                                onClick={onConfirmar}
                                className="btn btn-danger">
                                Sí, cancelar
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </>
    );
};

export default ModalConfirmacion;