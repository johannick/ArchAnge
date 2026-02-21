import { Api } from '@/api/apirest'

export const service = new Api();

service.baseUrl = 'api/';
service.version = import.meta.env.VITE_API_VERSION

export class StatusCodeError extends Error {
  /** @format int32 */
  public readonly statusCode: number;

  public constructor(status: number, message: string) {
    super(message);
    this.statusCode = status;
  }
}
