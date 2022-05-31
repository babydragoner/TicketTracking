import axios, { AxiosInstance, AxiosRequestConfig, AxiosResponse } from 'axios'
import type { RequestOptions, Result, UploadFileParams } from '/#/axios';
axios.defaults.baseURL = 'http://localhost:3000/'

export class apiHelper {
    
  private axiosInstance: AxiosInstance;
    token: string = '';
    constructor(config?: any) {

        this.axiosInstance = axios.create({
            headers: { 'Content-Type': 'application/json' },
            timeout: 60000,
            baseURL: import.meta.env.VITE_BACKEND_HOST //'https://localhost:44338/api/', // VUE_APP_BASE_API, 
            // transformRequest: [(data) => {
            //   let ret = ''
            //   const tempData = getJwtData(data)
            //   for (const it in tempData) {
            //     ret += encodeURIComponent(it) + '=' + encodeURIComponent(tempData[it]) + '&';
            //   }
            //   return ret
            // }]
          });

        // 設置 request 攔截器
        this.axiosInstance.interceptors.request.use((config: any) => {
          // token本身是會過期的，需要返回狀態查詢是不是過期
          config.headers.Authorization = 'Basic ' + this.token;
          return config
      })

        // 設置 response 攔截器
        // this.axiosInstance.interceptors.response.use(
        //     (response) => {
        //     // 如果回應200表示正常連線，可以返回資料結果，反之reject
        //     if (response.status === 200) {            
        //         return Promise.resolve(response);        
        //     } else {            
        //         return Promise.reject(response);        
        //         }    
        //     },

        //     // 根據不同的回應碼來訂製不同的錯誤訊息
        //     (error) => {
        //     if (error && error.response) {
        //         switch (error.response.status) {
        //         case 400:
        //             error.message = 'Request Error!'
        //             break
        //         case 401:
        //             error.message = 'No permission, need login.'
        //             break
        //         case 403:
        //             error.message = 'Access denied!'
        //             break
        //         case 404:
        //             // 自動帶入 request 地址的寫法
        //             error.message = `Address not exist: ${error.response.config.url}`
        //             break
        //         case 408:
        //             error.message = 'Request timeout!'
        //             break
        //         case 500:
        //             error.message = 'Server inside error!'
        //             break
        //         case 501:
        //             error.message = 'Service not allowed!'
        //             break
        //         case 502:
        //             error.message = 'Bad gateway!'
        //             break
        //         case 503:
        //             error.message = 'No service!'
        //             break
        //         case 504:
        //             error.message = 'Gateway timeout!'
        //             break
        //         case 505:
        //             error.message = 'http version not supported!'
        //             break
        //         default:
        //             break
        //         }
        //     }
        //     Message({
        //         message: error.message,
        //         type: 'error',
        //         duration: 5 * 1000
        //     })
        //     console.log('error',error)
        //     return Promise.reject(error)
        //     }
        // )
      }

      setToken(value: string) {
        this.token = value;
      }

      get2<T = any>(config: any, options?: any): Promise<T> {

        var conf = { url:config, method: 'GET' };
        // return this.request({ ...config, method: 'GET' }, options);
        return new Promise((resolve, reject) => {
            this.axiosInstance(conf, {
              params: options
            })
              .then((res: any) => {
                // resolve(response.data)
                resolve(res as unknown as Promise<T>);
              })
              .catch((error: any) => {
                reject(error)
              })
          })
        // return new Promise((resolve, reject) => {
        //     this.axiosInstance.get(config, {
        //       params: options
        //     })
        //       .then((res: any) => {
        //         // resolve(response.data)
        //         resolve(res as unknown as Promise<T>);
        //       })
        //       .catch((error: any) => {
        //         reject(error)
        //       })
        //   })
      }

      get<T = any>(config: any, options?: any): Promise<T> {
        return this.request<T>({ ...config, method: 'GET' }, options);
      }
  
      post<T = any>(config: any, options?: any): Promise<T> {
        return this.request<T>({ ...config, method: 'POST' }, options);
      }
      put<T = any>(config: any, options?: any): Promise<T> {
        return this.request<T>({ ...config, method: 'PUT' }, options);
      }
      delete<T = any>(config: any, options?: any): Promise<T> {
        return this.request<T>({ ...config, method: 'DELETE' }, options);
      }

    // async post(url: string, params: any){
    //     const { data } = await this.axiosInstance.post(
    //         url,
    //         params,
    //         {
    //           headers: {
    //             'Content-Type': 'application/json',
    //             Accept: 'application/json',
    //           },
    //         },
    //       );
    //     return data;
    //     // return new Promise((resolve, reject) => {
    //     //   axios.post(url, {
    //     //     params: params
    //     //   })
    //     //     .then((response: any) => {
    //     //       resolve(response.data)
    //     //     })
    //     //     .catch((error: any) => {
    //     //       reject(error)
    //     //     })
    //     // })
    //   }

      request<T = any>(config: AxiosRequestConfig, options?: RequestOptions): Promise<T> {
        let conf = config;
        // let conf: CreateAxiosOptions = cloneDeep(config);
        // const transform = this.getTransform();
    
        // const { requestOptions } = this.options;
    
        // const opt: RequestOptions = Object.assign({}, requestOptions, options);
    
        // const { beforeRequestHook, requestCatchHook, transformRequestHook } = transform || {};
        // if (beforeRequestHook && isFunction(beforeRequestHook)) {
        //   conf = beforeRequestHook(conf, opt);
        // }
        // conf.requestOptions = options;
    
        // conf = this.supportFormData(conf);
    
        return new Promise((resolve, reject) => {
          this.axiosInstance
          .request<any, AxiosResponse<Result>>(conf)
          .then((res: AxiosResponse<Result>) => {
            //   if (transformRequestHook && isFunction(transformRequestHook)) {
            //     try {
            //       const ret = transformRequestHook(res, opt);
            //       resolve(ret);
            //     } catch (err) {
            //       reject(err || new Error('request error!'));
            //     }
            //     return;
            //   }
              resolve(res.data as unknown as Promise<T>);
            })
            .catch((e: any) => {
            //   if (requestCatchHook && isFunction(requestCatchHook)) {
            //     reject(requestCatchHook(e, opt));
            //     return;
            //   }
            //   if (axios.isAxiosError(e)) {
            //     // rewrite error message from axios in here
            //   }
              reject(e);
            });
        });
      }
}
export const apiHelper2 = {

    service: () => {
      },

//   const service = axios.created({
//     headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
//     timeout: 60000,

//     transformRequest: [(data) => {
//       let ret = ''
//       const tempData = getJwtData(data)
//       for (const it in tempData) {
//         ret += encodeURIComponent(it) + '=' + encodeURIComponent(tempData[it]) + '&';
//       }
//       return ret
//     }]
//   })

  get(url: string, params: any){
    return new Promise((resolve, reject) => {
      axios.get(url, {
        params: params
      })
        .then((response: any) => {
          resolve(response.data)
        })
        .catch((error: any) => {
          reject(error)
        })
    })
  },

//   post: (url, params) => {
//     return new Promise((resolve, reject) => {
//       axios.post(url, params)
//         .then((response) => {
//           resolve(response.data)
//         })
//         .catch((error) => {
//           reject(error)
//         })
//     })
//   },

//   put: (url, params) => {
//     return new Promise((resolve, reject) => {
//       axios.put(url, params)
//         .then((response) => {
//           resolve(response.data)
//         })
//         .catch((error) => {
//           reject(error)
//         })
//     })
//   },


//   delete: (url, params) => {
//     return new Promise((resolve, reject) => {
//       axios.delete(url)
//         .then((response) => {
//           resolve(response.data)
//         })
//         .catch((error) => {
//           reject(error)
//         })
//     })
//   }
}


export const defHttp = new apiHelper();