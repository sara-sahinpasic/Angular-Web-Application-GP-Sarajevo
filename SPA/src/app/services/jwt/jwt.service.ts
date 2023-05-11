import { Injectable } from '@angular/core';
import { InvalidArgumentException } from 'src/app/exceptions/InvalidArgumentException';
import { Buffer } from 'buffer';

@Injectable({
  providedIn: 'root'
})
export class JwtService {

  constructor() { }

  decode(token: string): string {
    const tokenParts = token.split(".");

    if (tokenParts.length < 3) {
      throw new InvalidArgumentException(["token"]);
    }

    const payload: string = tokenParts[1];
    const payloadDecoded = Buffer.from(payload, "base64").toString();

    return payloadDecoded
  }
}
