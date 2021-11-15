const app =  new Vue({
    el: '#app',
    data() {
      return {
          users:[],
          user:{
              userName:'',
              Email:'',
              Password:'',
          },
          defaultUser:{
            userName:'',
            Email:'',
            Password:'',
          }
      }
    },
    created(){
        this.loadData()
    },
    methods:{
        loadData(){
            fetch('./api/user')
                .then(response => response.json())
                .then(response => {
                    this.users = response.data
                })
                // ajax not working           
            },
        updating(user){
            this.user = {...user}
            $('#modal').modal('show')
        },
        upsert(){
            let operation = 'POST'
            const user = {
                userName : this.user.userName,
                Email : this.user.email,
                Password : this.user.password,
            }
            if (this.user.userId){
                user.userId = this.user.userId                
                operation = 'PUT'
            }        
            fetch('./api/user',{
                method: operation, 
                headers: {
                  'Content-Type': 'application/json',
                },
                body: JSON.stringify(user),
            })
            .then(response => response.json())
            .then(response => {
                this.user = this.defaultUser
                this.loadData()
                $('#modal').modal('hide')
            })
        },
        remove(userId){
            fetch('./api/user/'+userId,{ method:'delete'})
            .then(response => response.json())
            .then(response => {
                this.loadData()
                toastr.options = {
                    timeOut: "1000",
                }
                toastr.success('Deleted')
            })
        }
    }
})
