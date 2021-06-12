var app = new Vue({
    el: '#app',
    data: {
        selected: 'selected',
        companyRecruiterList: [],
        message: 'Message From View',
        options: [
            { text  : '', value : ''}
        ]
    },
    methods: {
        onCompanyChange: function onCompanyChange(event) {
            this.selected = event.target.value;
            //Call and populate recruiters based on the selected option
            axios.get("http://localhost:5000/Api/GetByCompanyId/" + this.selected).then(response => {
                this.companyRecruiterList = response.data
                //Clear an array every time a new company is selected
                this.options = [];

                //console.log(this.companyRecruiterList);
                this.companyRecruiterList.forEach((obj) => {
                    this.options.push({ text: obj.firstName, value: obj.id});
                    //this.options.value.push(obj.id);
                    //console.log(obj.id);
                })
            })

            //console.log(this.selected);
        }
    },
    mounted() {
        //console.log("Load Mounted");
        //Preselect first option and select 

    },
    created() {
        //console.log("Load Created");
    }
})