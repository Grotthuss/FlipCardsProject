import React, { useState } from 'react';
import './AddCardSet.css';
import './errorEnums.ts';
import {Errors} from "./errorEnums";

interface AddCardSetProps {
    onAdd: (setName: string) => void;
}

const AddCardSet: React.FC<AddCardSetProps> = ({ onAdd }) => {
    const [setName, setSetName] = useState('');
    const [error, setError] = useState<string | null>(null);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError(null);

        if (!setName) {
            setError(Errors.NAME);
            return;
        }

        try {
            const response = await fetch(`https://localhost:44372/api/Home/${setName}/CreateEmptySet`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify([]),
            });

            if (!response.ok) {
                throw new Error(Errors.CREATE_SET);
            }

            onAdd(setName);
            setSetName('');
        } catch (err) {
            setError(Errors.CREATE_SET + (err as Error).message);
        }
    };

    return (
        <form className="add-card-set-container" onSubmit={handleSubmit}>
            <h2>Add New Card Set</h2>

            <input
                type="text"
                placeholder="Enter card set name"
                value={setName}
                onChange={(e) => setSetName(e.target.value)}
            />

            <button type="submit">Add Card Set</button>
        </form>
    );
};

export default AddCardSet;
