(() => {


    const Carro = {
        tabla: null,
        init() {
            this.inicializarTabla();
           
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
                    { data: 'marca' }

                ]
            });
        }


    };






    $(document).ready(() => Carro.init());

})();

