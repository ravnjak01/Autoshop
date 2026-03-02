import test from 'node:test';
import { isValidEmail } from './email-validator'; // Putanja do tvog fajla

describe('Email Validator Testovi', () => {
    
    test('Treba vratiti true za ispravan email', () => {
        const result = isValidEmail('kupac@autoshop.ba');
        expect(result).toBe(true);
    });

    test('Treba vratiti false ako fali @ znak', () => {
        const result = isValidEmail('neispravan.email.com');
        expect(result).toBe(false);
    });

    test('Treba vratiti false ako nema domene (npr. .com)', () => {
        const result = isValidEmail('ime@prezime');
        expect(result).toBe(false);
    });

    test('Treba vratiti false za prazan unos', () => {
        const result = isValidEmail('');
        expect(result).toBe(false);
    });
});