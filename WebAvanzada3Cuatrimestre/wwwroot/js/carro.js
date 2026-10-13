(() => {


    const Carro = {
        tabla: null,
        init() {
            this.inicializarTabla();
            this.registrarEventos();
        },
        inicializarTabla() {
            this.tabla = new DataTable('#tblCarro',{
                ajax: {
                    url: '/Carro/ObtenerCarros',
                    dataSrc: '',
                    type: 'GET'
                },
                columns: [
                    { data: 'id' },
                    { data: 'placa' },
                    { data: 'marca' },
                    { data: 'duenno.nombre' },
                    {
                        data: null,
                        title: 'Acciones',
                        className: 'dt-actions-cell',
                        render: (data, type, row) => {
                            return `<div class="table-actions">
                                        <button type="button" class="btn-action btn-action-edit btn-editar" data-id="${row.id}" aria-label="Editar vehículo" title="Editar">
                                            <i class="bi bi-pencil-fill" aria-hidden="true"></i>
                                        </button>
                                        <button type="button" class="btn-action btn-action-delete btn-eliminar" data-id="${row.id}" aria-label="Eliminar vehículo" title="Eliminar">
                                            <i class="bi bi-trash3-fill" aria-hidden="true"></i>
                                        </button>
                                    </div>`;

                        }
                    }
                    

                ],
                language: {
                    url: 'https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json'
                }

                // Agrega aquí cualquier otra configuración de DataTable que necesites
            });
        },
        registrarEventos() {


            $('#btnGuardarCarro').on('click', function () {
                Carro.guardarCarro();
            })

            $('#btnEditarCarro').on('click', function () {
                Carro.editarCarro();
            })



            $('#tblCarro').on('click', '.btn-eliminar', function () {
                const id = $(this).data('id');
                Carro.eliminarCarro(id);
            })
            $('#tblCarro').on('click', '.btn-editar', function () {
                const id = $(this).data('id');
                Carro.cargarCarro(id);
            })


        },
        guardarCarro() {
            let form = $('#formCrearCarro');

            // if (!form.valid()) {
            //     return;
            // }

            $.ajax({
                url: form.attr('action'),
                type: 'POST',
                data: form.serialize(),
                success: function (respuesta) { // Las reglas de negocio son procesos correctos, ya sean que se apliquen sus reglas o no. Por eso las reglas de negocio no pueden ser excepciones.

                    if (respuesta.esCorrecto) {
                        Swal.fire({
                            title: "Good job!",
                            text: respuesta.mensaje,
                            icon: "success"
                        });
                        $('#modalCrearCarro').modal('hide');
                        form[0].reset();
                        Carro.tabla.ajax.reload();
                    }
                    else {
                        //Libreria para manejar los mensajes emergentes
                        Swal.fire({ 
                            icon: "error",
                            title: "Oops...",
                            text: respuesta.mensaje,
                          
                        });

                    }





                },
                error: function (respuesta) { // Las excepcioneso errores no controlados caen en este apartado

                }




            })






        },

        eliminarCarro(id) {

            Swal.fire({
                title: "Estas seguro?",
                text: "No podras revertir esta operacion",
                icon: "warning",
                showCancelButton: true,
                confirmButtonText: "Si, eliminar",
            }).then((result) => {

                if (result.isConfirmed) {

                    $.ajax({
                        url: `/Carro/EliminarCarro?id=${id}`,
                        type: 'DELETE',
                        success: function (respuesta) {
                            if (respuesta.esCorrecto) {
                                Swal.fire({
                                    title: "Good job!",
                                    text: respuesta.mensaje,
                                    icon: "success"
                                });
                                Carro.tabla.ajax.reload();
                            }
                            else {
                                //Libreria para manejar los mensajes emergentes
                                Swal.fire({
                                    icon: "error",
                                    title: "Oops...",
                                    text: respuesta.mensaje,

                                });

                            }
                        },
                        error: function () {

                        }
                    })
                }



            });

        },

        cargarCarro(id) {


            $.get(`/Carro/ObtenerCarroPorId?id=${encodeURIComponent(id)}`, function (resultado) {
                if (resultado != null) {
                    $('#formEditarCarro #Id').val(resultado.id);
                    $('#formEditarCarro #Placa').val(resultado.placa);
                    $('#formEditarCarro #Marca').val(resultado.marca);
                    $('#formEditarCarro #Fkduenno').val(resultado.fkduenno ?? '');

                    $('#modalEditarCarro').modal('show');
                }
            });


        },

        editarCarro(id) {
            let form = $('#formEditarCarro');

            // if (!form.valid()) {
            //     return;
            // }

            $.ajax({
                url: form.attr('action'),
                type: 'POST',
                data: form.serialize(),
                success: function (respuesta) { // Las reglas de negocio son procesos correctos, ya sean que se apliquen sus reglas o no. Por eso las reglas de negocio no pueden ser excepciones.

                    if (respuesta.esCorrecto) {
                        Swal.fire({
                            title: "Good job!",
                            text: respuesta.mensaje,
                            icon: "success"
                        });
                        $('#modalEditarCarro').modal('hide');
                        form[0].reset();
                        Carro.tabla.ajax.reload();
                    }
                    else {
                        //Libreria para manejar los mensajes emergentes
                        Swal.fire({
                            icon: "error",
                            title: "Oops...",
                            text: respuesta.mensaje,

                        });

                    }





                },
                error: function (respuesta) { // Las excepcioneso errores no controlados caen en este apartado

                }
            })
        }


    };






    $(document).ready(() => Carro.init());

})();

