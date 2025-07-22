import { ReactNode, FormEvent } from 'react';

interface ValidateFormProps {
  children: ReactNode;
  onSubmit: (isOk: boolean) => void;
}

export default function ValidateForm({ children, onSubmit }: ValidateFormProps) {
  const handleSubmit = (e: FormEvent) => {
    e.preventDefault();
    
    const form = e.currentTarget as HTMLFormElement;
    const requiredTextarea = Array.from(form.querySelectorAll('textarea[required]')) as HTMLInputElement[];
    const requiredInputs = Array.from(form.querySelectorAll('input[required]')) as HTMLInputElement[];

    const invalidList = [...requiredInputs, ...requiredTextarea].filter(input => input.value.trim() === '');
    onSubmit(invalidList.length === 0);
  };

  return (
    <form noValidate onSubmit={handleSubmit}>
      {children}
    </form>
  );
}